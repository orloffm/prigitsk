using System.Collections.Generic;

namespace OrlovMikhail.GitTools.Loading.Client.Repository
{
    public class RepositoryDataBuilder : IRepositoryDataBuilder
    {
        private readonly Dictionary<string, NodeRecord> _records;

        private class NodeRecord
        {
            public NodeRecord(string hash)
            {
                Hash = hash;
                Parents = new List<string>();
                Branches = new List<string>();
                Tags = new List<string>();
            }

            public string Hash { get; private set; }
            public List<string> Parents { get; private set; }
            public List<string> Branches { get; private set; }
            public List<string> Tags { get; private set; }
            public string Description { get; set; }
        }

        public RepositoryDataBuilder()
        {
            _records = new Dictionary<string, NodeRecord>();
        }

        private NodeRecord GetRecordForHash(string hash)
        {
            NodeRecord node;
            if (!_records.TryGetValue(hash, out node))
            {
                node = new NodeRecord(hash);
                _records.Add(hash, node);
            }
            return node;
        }

        public void AddCommit(string hash, string[] parentHashes)
        {
            NodeRecord record = GetRecordForHash(hash);
            record.Parents.AddRange(parentHashes);
        }

        public void AddCommitDescription(string hash, string description)
        {
            NodeRecord record = GetRecordForHash(hash);
            record.Description = description;
        }

        public void AddRemoteBranch(string friendlyName, string sourceHash)
        {
            NodeRecord record = GetRecordForHash(sourceHash);
            record.Branches.Add(friendlyName);
        }

        public void AddTag(string friendlyName, string sourceHash)
        {
            NodeRecord record = GetRecordForHash(sourceHash);
            record.Tags.Add(friendlyName);
        }

        public IRepositoryState Build()
        {
            var nodes = new Dictionary<string, CommitInfo>();

            foreach (NodeRecord nodeRecord in _records.Values)
            {
                CreateNode(nodeRecord.Hash, nodes, _records);
            }

            IRepositoryState ret = new RepositoryState(nodes.Values);
            return ret;
        }

        private CommitInfo CreateNode(string currentHash, Dictionary<string, CommitInfo> targetDictionary,
            Dictionary<string, NodeRecord> allRecords)
        {
            CommitInfo result;

            if (targetDictionary.TryGetValue(currentHash, out result))
            {
                // Already created.
                return result;
            }

            NodeRecord record = allRecords[currentHash];

            // Create children.
            var parents = new List<CommitInfo>();
            foreach (string parentRecordHash in record.Parents)
            {
                CommitInfo parent = CreateNode(parentRecordHash, targetDictionary, allRecords);
                parents.Add(parent);
            }

            result = new CommitInfo(currentHash, parents, record.Branches, record.Tags, record.Description);
            targetDictionary.Add(currentHash, result);
            return result;
        }
    }
}