namespace types {
    public class Either<TValue, TError> {
        public TValue? Value { get; }
        public TError? Error { get; }
        public bool IsSuccess { get; }

        public Either(TValue value)
        {
            Value = value;
            IsSuccess = true;
        }

        public Either(TError error)
        {
            Error = error;
            IsSuccess = false;
        }
    }
}