// Track the content of the sea
class Sea {
    // the sea is a square grid of tiles. It is a one-dimensional array, and this
    // class maps x and y coordinates to an array index
    // Each tile is either empty (value of tiles at index is 0)
    // or contains a ship (value of tiles at index is the ship number)
    private int tiles[];

    private int size;

    public Sea(int make_size) {
        size = make_size;
        tiles = new int[size*size];
    }

    public int size() { return size; }

    // This writes out a representation of the sea, but in a funny order
    // The idea is to give the player the job of working it out
    public String encodedDump() {
        StringBuilder out = new StringBuilder();
        for (int x = 0; x < size; ++x) {
            for (int y = 0; y < size; ++y)
                out.append(Integer.toString(get(x, y)));
            out.append('\n');
        }
        return out.toString();
    }

    /* return true if x,y is in the sea and empty
     * return false if x,y is occupied or is out of range
     * Doing this in one method makes placing ships much easier
     */
    public boolean isEmpty(int x, int y) {
        if ((x<0)||(x>=size)||(y<0)||(y>=size)) return false;
        return (get(x,y) == 0);
    }

    /* return the ship number, or zero if no ship.
     * Unlike isEmpty(x,y), these other methods require that the
     * coordinates passed be valid
     */
    public int get(int x, int y) {
        return tiles[index(x,y)];
    }

    public void set(int x, int y, int value) {
        tiles[index(x, y)] = value;
    }

    // map the coordinates to the array index
    private int index(int x, int y) {
        if ((x < 0) || (x >= size))
            throw new ArrayIndexOutOfBoundsException("Program error: x cannot be " + x);
        if ((y < 0) || (y >= size))
            throw new ArrayIndexOutOfBoundsException("Program error: y cannot be " + y);

        return y*size + x;
    }
}
