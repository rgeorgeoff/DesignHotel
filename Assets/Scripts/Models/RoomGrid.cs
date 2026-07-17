namespace Models
{
    [System.Serializable]
    public class RoomGrid
    {
        [System.Serializable]
        public class RoomRow
        {
            public RoomData[] Cols; // The wrapped array.
        }

        public RoomRow[] Rows; // The 2D array.

        public RoomData this[int rowIndex, int colIndex]
        {
            get { return Rows[rowIndex].Cols[colIndex]; }
            set { Rows[rowIndex].Cols[colIndex] = value; }
        }
    }
}