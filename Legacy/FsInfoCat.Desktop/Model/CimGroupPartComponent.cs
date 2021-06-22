namespace FsInfoCat.Desktop.Model
{
    public class CimGroupPartComponent<TGroup, TPart>
    {
        public CimGroupPartComponent(TGroup group, TPart part)
        {
            GroupComponent = group;
            PartComponent = part;
        }

        public TGroup GroupComponent { get; }
        public TPart PartComponent { get; }
    }
}
