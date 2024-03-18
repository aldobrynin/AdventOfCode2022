namespace AoC2019;

public class IntCodeComputer {
    private readonly Dictionary<long, long> _program;
    private int _i;
    private long _relativeBase;
    private Action<long>? _onOutput;
    public ComputerInput ComputerInput { get; }

    public IntCodeComputer(long[] program, long[]? input = null) {
        _program = program.WithIndex().ToDictionary(x => (long)x.Index, x => x.Element);
        ComputerInput = new(input ?? []);
    }

    public void AddInput(long input) => ComputerInput.Add(input);
    public void AddInput(params long[] input) {
        foreach (var value in input) ComputerInput.Add(value);
    }

    public void AddAsciiInput(string input) {
        Console.WriteLine(input);
        foreach (var value in input.Append('\n')) AddInput(value);
    }

    public bool IsWaitingForInput => ComputerInput.IsWaiting;

    public long Output { get; private set; }
    public bool IsHalted { get; private set; }

    public async Task<long> GetNextOutput() {
        if (!await RunToNextOutput()) throw new Exception("Unexpected end of program");
        return Output;
    }

    public async Task<long> RunToEnd() {
        while (await RunToNextOutput()) {
        }

        return Output;
    }

    public void PipeOutputTo(Action<long>? onOutput) => _onOutput = onOutput;

    public async Task<bool> RunToNextOutput() {
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
                    var input = await ComputerInput.GetNext();
                    WriteNext(input, mode1);
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

    public async IAsyncEnumerable<long> ReadAllOutputs() {
        while (await RunToNextOutput())
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

public class ComputerInput {
    private readonly Queue<long> _inputQueue;
    private TaskCompletionSource<long>? _waitTaskCompletionSource;

    public ComputerInput(params long[] values) {
        _inputQueue = new Queue<long>(values);
    }

    public event Func<long>? OnInput;

    public bool IsWaiting => _waitTaskCompletionSource is not null;

    public void Add(long value) {
        if (_waitTaskCompletionSource is null) {
            _inputQueue.Enqueue(value);
        }
        else {
            var tcs = _waitTaskCompletionSource;
            _waitTaskCompletionSource = null;
            tcs.SetResult(value);
        }
    }

    public async Task<long> GetNext() {
        if (_inputQueue.Count > 0) return _inputQueue.Dequeue();
        if (OnInput is not null) return OnInput();
        if (_waitTaskCompletionSource is not null) throw new Exception("Unexpected state");
        _waitTaskCompletionSource = new();
        return await _waitTaskCompletionSource.Task;
    }
}
