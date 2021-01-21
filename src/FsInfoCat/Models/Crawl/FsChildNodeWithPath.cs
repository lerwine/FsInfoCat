using System;
using System.Collections.ObjectModel;

namespace FsInfoCat.Models.Crawl
{
    public sealed class FsChildNodeWithPath
    {
        public IFsChildNode ChildNode { get; }
        public string Path { get; }
        private FsChildNodeWithPath(string directoryName, IFsChildNode childNode)
        {
            Path = (string.IsNullOrEmpty(directoryName)) ? childNode.Name : System.IO.Path.Combine(directoryName, childNode.Name);
            ChildNode = childNode;
        }

        public Collection<FsChildNodeWithPath> Create(FsRoot root)
        {
            Collection<FsChildNodeWithPath> result = new Collection<FsChildNodeWithPath>();
            Create(root.RootPathName, root.ChildNodes, result);
            return result;
        }

        private void Create(string rootPathName, Collection<IFsChildNode> childNodes, Collection<FsChildNodeWithPath> result)
        {
            foreach (IFsChildNode c in childNodes)
            {
                FsChildNodeWithPath item = new FsChildNodeWithPath(rootPathName, c);
                result.Add(item);
                if (c is IFsDirectory)
                    Create(item.Path, ((IFsDirectory)c).ChildNodes, result);
            }
        }
    }
}
