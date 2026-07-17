namespace Models
{
    [System.Serializable]
    public class Grid<T>
    {
        [System.Serializable]
        public class TRow
        {
            public T[] Cols; // The wrapped array.
        }

        public TRow[] Rows; // The 2D array.

        public T this[int rowIndex, int colIndex]
        {
            get { return Rows[rowIndex].Cols[colIndex]; }
            set { Rows[rowIndex].Cols[colIndex] = value; }
        }
    }
}