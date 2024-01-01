namespace AoC2019;

public class IntCodeComputer {
    private readonly Dictionary<long, long> _program;
    private readonly Queue<long> _input;
    private int _i;
    private long _relativeBase;
    private Action<long>? _onOutput;

    public IntCodeComputer(long[] program, long[]? input = null) {
        _program = program.WithIndex().ToDictionary(x => (long)x.Index, x => x.Element);
        _input = new(input ?? Array.Empty<long>());
        OnInput = () => _input.Dequeue();
    }

    public delegate long InputProvider();

    public InputProvider OnInput { get; init; }

    public void AddInput(long input) => _input.Enqueue(input);

    public long Output { get; private set; }
    public bool IsHalted { get; private set; }

    public long GetNextOutput() {
        if (!RunToNextOutput()) throw new Exception("Unexpected end of program");
        return Output;
    }

    public long RunToEnd() {
        while (RunToNextOutput()) {
        }

        return Output;
    }

    public void PipeOutputTo(Action<long>? onOutput) => _onOutput = onOutput;

    public bool RunToNextOutput() {
        while (true) {
            var opcode = ReadNext();
            var (modes, op) = Math.DivRem(opcode, 100);
            var mode1 = modes % 10;
            var mode2 = modes / 10 % 10;
            var mode3 = modes / 100 % 10;
            switch (op) {
                case 1: {
                    WriteNext(ReadNext(mode1) + ReadNext(mode2), mode3);
                    break;
                }
                case 2: {
                    WriteNext(ReadNext(mode1) * ReadNext(mode2), mode3);
                    break;
                }
                case 3:
                    WriteNext(OnInput(), mode1);
                    break;
                case 4:
                    Output = ReadNext(mode1);
                    _onOutput?.Invoke(Output);
                    return true;
                case 5: {
                    var firstOperand = ReadNext(mode1);
                    var secondOperand = ReadNext(mode2);
                    if (firstOperand != 0) _i = (int)secondOperand;
                    break;
                }
                case 6: {
                    var firstOperand = ReadNext(mode1);
                    var secondOperand = ReadNext(mode2);
                    if (firstOperand == 0) _i = (int)secondOperand;
                    break;
                }
                case 7: {
                    WriteNext(ReadNext(mode1) < ReadNext(mode2) ? 1 : 0, mode3);
                    break;
                }
                case 8: {
                    WriteNext(ReadNext(mode1) == ReadNext(mode2) ? 1 : 0, mode3);
                    break;
                }
                case 9:
                    _relativeBase += ReadNext(mode1);
                    break;
                case 99:
                    IsHalted = true;
                    return false;
                default:
                    throw new Exception($"Unexpected opcode {op} at position {_i}");
            }
        }
    }
    
    public IEnumerable<long> ReadAllOutputs() {
        while (RunToNextOutput())
            yield return Output;
    }
    
    private void WriteNext(long value, long mode = 1) {
        var address = ReadNext();
        var addressValue = mode switch {
            0 => address,
            2 => _relativeBase + address,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
        _program[addressValue] = value;
    }

    private long ReadNext(long mode = 1) {
        var value = _program.GetValueOrDefault(_i++);
        return mode switch {
            0 => _program.GetValueOrDefault(value),
            1 => value,
            2 => _program.GetValueOrDefault(_relativeBase + value),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }
}