namespace RNG
{
    interface Dice
    {
        public int roll();
    }
}

class D4 : RNG.Dice
{
    public int roll()
    {
        Random rnd = new Random();
        return rnd.Next(1, 5);
    }
}

class D6 : RNG.Dice
{
    public int roll()
    {
        Random rnd = new Random();
        return rnd.Next(1, 7);
    }
}

class D8 : RNG.Dice
{
    public int roll()
    {
        Random rnd = new Random();
        return rnd.Next(1, 9);
    }
}

class D12 : RNG.Dice
{
    public int roll()
    {
        Random rnd = new Random();
        return rnd.Next(1, 12);
    }
}

class D20 : RNG.Dice
{
    public int roll()
    {
        Random rnd = new Random();
        return rnd.Next(1, 20);
    }
}