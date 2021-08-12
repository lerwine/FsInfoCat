namespace FsInfoCat.Desktop.ViewModel
{
    public class ChangeStateTracker : ValidationStateTracker
    {
        public void SetChangeState(string propertyName, bool isChanged) => SetValidationState(propertyName, isChanged);
    }
}
