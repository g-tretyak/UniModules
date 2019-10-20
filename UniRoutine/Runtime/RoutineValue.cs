namespace UniTools.UniRoutine.Runtime
{
    public struct RoutineValue
    {
        public readonly int Id;

        public RoutineValue(int id)
        {
            Id = id;
        }

        public override int GetHashCode() => Id;

        public bool Equals(RoutineValue obj) => Id == obj.Id;

        public override bool Equals(object obj) 
        {
            if (obj is RoutineValue value)
                return Id == value.Id;

            // compare elements here
            return false;
        }
    }
}