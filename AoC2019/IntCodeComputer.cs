namespace AoC2019;

public class IntCodeComputer(long[] program, long[]? input = null) {
    private readonly long[] _program = program.ToArray();
    private readonly Queue<long> _input = new(input ?? Array.Empty<long>());
    private int _i;
    private Action<long>? _onOutput;

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
        while (_i < _program.Length) {
            var opcode = ReadNext(isImmediateMode: true);
            var (modes, op) = Math.DivRem(opcode, 100);
            var mode1 = modes % 10 == 1;
            var mode2 = modes / 10 % 10 == 1;
            switch (op) {
                case 1: {
                    var firstOperand = ReadNext(mode1);
                    var secondOperand = ReadNext(mode2);
                    _program[ReadNext()] = firstOperand + secondOperand;
                    break;
                }
                case 2: {
                    var firstOperand = ReadNext(mode1);
                    var secondOperand = ReadNext(mode2);
                    _program[ReadNext()] = firstOperand * secondOperand;
                    break;
                }
                case 3:
                    _program[ReadNext()] = _input.Dequeue();
                    break;
                case 4:
                    Output = _program[ReadNext()];
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
                    var firstOperand = ReadNext(mode1);
                    var secondOperand = ReadNext(mode2);
                    _program[ReadNext()] = firstOperand < secondOperand ? 1 : 0;
                    break;
                }
                case 8: {
                    var firstOperand = ReadNext(mode1);
                    var secondOperand = ReadNext(mode2);
                    _program[ReadNext()] = firstOperand == secondOperand ? 1 : 0;
                    break;
                }
                case 99:
                    IsHalted = true;
                    return false;
                default:
                    throw new Exception($"Unexpected opcode {op} at position {_i}");
            }
        }

        throw new Exception("Unexpected end of program");
    }

    private long ReadNext(bool isImmediateMode = true) {
        var value = _program[_i++];
        return isImmediateMode ? value : _program[value];
    }
}