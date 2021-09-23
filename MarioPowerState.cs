public interface IMarioPowerState
{
    //For future use
    void FireFlower();
    void Mushroom();
    void TakeDamage();

    //For sprint 1 proof of concept
    void SmallMario();
    void BigMario();
    void FlameMario();
}

public class MarioPower
{
    public IMarioPowerState state;

    public MarioPower()
    {
        state = new StandardMario(this);
    }

    //COMMON METHODS
}

public class StandardMario : IMarioPowerState
{
    private MarioPower mario;

    public StandardMario(MarioPower mario)
    {
        this.mario = mario;

        //Construct sprite
    }

    public void FireFlower()
    {
        mario.state = new SuperMario(mario);
    }

    public void Mushroom()
    {
        mario.state = new SuperMario(mario);
    }

    public void TakeDamage()
    {
        mario.state = new DeadMario(mario);
    }

    public void SmallMario()
    {
        //Do nothing, already Standard Mario
    }

    public void BigMario()
    {
        mario.state = new SuperMario(mario);
    }

    public void FlameMario()
    {
        mario.state = new FireMario(mario);
    }
}

public class SuperMario : IMarioPowerState
{
    private MarioPower mario;

    public SuperMario(MarioPower mario)
    {
        this.mario = mario;

        //Construct sprite
    }

    public void FireFlower()
    {
        mario.state = new FireMario(mario);
    }

    public void Mushroom()
    {
        //Do nothing, already Super Mario
    }

    public void TakeDamage()
    {
        mario.state = new StandardMario(mario);
    }

    public void SmallMario()
    {
        mario.state = new StandardMario(mario);
    }

    public void BigMario()
    {
        //Do nothing, already Super Mario
    }

    public void FlameMario()
    {
        mario.state = new FireMario(mario);
    }
}

public class FireMario : IMarioPowerState
{
    private MarioPower mario;

    public FireMario(MarioPower mario)
    {
        this.mario = mario;

        //Construct sprite
    }

    public void FireFlower()
    {
        //Do nothing, already Fire Mario
    }

    public void Mushroom()
    {
        //Do nothing, already Fire Mario
    }

    public void TakeDamage()
    {
        mario.state = new SuperMario(mario);
    }

    public void SmallMario()
    {
        mario.state = new StandardMario(mario);
    }

    public void BigMario()
    {
        mario.state = new SuperMario(mario);
    }

    public void FlameMario()
    {
        //Do nothing, already Fire Mario
    }
}

public class DeadMario : IMarioPowerState
{
    private MarioPower mario;

    public DeadMario(MarioPower mario)
    {
        this.mario = mario;

        //Construct sprite
    }

    public void FireFlower()
    {
        //Do nothing, dead
    }

    public void Mushroom()
    {
        //Do nothing, dead
    }

    public void TakeDamage()
    {
        //Do nothing, dead
    }

    public void SmallMario()
    {
        //Do nothing, dead
    }

    public void BigMario()
    {
        //Do nothing, dead
    }

    public void FlameMario()
    {
        //Do nothing, dead
    }
}
