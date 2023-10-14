using System.Collections;

public abstract class GameState
{
    public GameplayManager Manager { get; private set; }

    public GameState(GameplayManager gameplayManager)
    {
        Manager = gameplayManager;
    }

    public virtual IEnumerator StartState()
    {
        yield return null;
    }

    public virtual IEnumerator CallForCutscene()
    {
        yield return null;
    }

    public virtual IEnumerator CallForModal(Modal modal)
    {
        yield return null;
    }

    public virtual IEnumerator PrimaryButton()
    {
        yield return null;
    }

    public virtual IEnumerator SecondaryButton() 
    { 
        yield return null;
    }

    public virtual IEnumerator RunButton()
    {
        yield return null;
    }

    public virtual IEnumerator CallForInventory()
    {
        yield return null;
    }

    public virtual void LeaveState()
    {
        return;
    }
}