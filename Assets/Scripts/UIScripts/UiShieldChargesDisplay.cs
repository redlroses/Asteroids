public class UiShieldChargesDisplay : UiDisplay
{
    private void OnEnable()
    {
        Shield.OnChargesChanges += DisplayText;
    }

    private void OnDisable()
    {
        Shield.OnChargesChanges -= DisplayText;
    }

}
