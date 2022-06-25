public void Enable()
{
    _effects.StartEnableAnimation();
}

public void Disable() 
{
    _pool.Free(this);
}