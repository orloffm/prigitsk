using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;
using OrlovMikhail.GitTools.Loading.Client.Common;
using Branch = OrlovMikhail.GitTools.Loading.Client.Common.Branch;
using Commit = OrlovMikhail.GitTools.Loading.Client.Common.Commit;
using Tag = OrlovMikhail.GitTools.Loading.Client.Common.Tag;

namespace OrlovMikhail.GitTools.Loading
{
    public class LibGit2Client : IGitClient
    {
        private readonly string _repositoryPath;
        private LibGit2Sharp.Repository _repository;

        public LibGit2Client(string repositoryPath)
        {
            this._repositoryPath = repositoryPath;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        string AbbreviateHash(GitObject source)
        {
            return source.Id.Sha.Substring(0, 7);
        }

        public void Initialise()
        {
            _repository = new Repository(_repositoryPath);
        }

        public IEnumerable<Commit> Commits
        {
            get
            {
                foreach (LibGit2Sharp.Commit c in _repository.Commits)
                {
                    Commit ret = new Commit();
                    ret.Hash = AbbreviateHash(c);
                    ret.ParentHashes = c.Parents.Select(AbbreviateHash).ToArray();
                    ret.Description = c.Message;
                    yield return ret;
                }
            }
        }

        public IEnumerable<Branch> Branches
        {
            get
            {
                foreach (LibGit2Sharp.Branch b in _repository.Branches)
                {
                    Branch ret = new Branch();
                    ret.Name = b.CanonicalName;
                    ret.IsRemote = b.IsRemote;
                    ret.TargetCommitHash = AbbreviateHash(b.Tip);
                    yield return ret;
                }
            }
        }

        public IEnumerable<Tag> Tags
        {
            get
            {
                foreach (LibGit2Sharp.Tag t in _repository.Tags)
                {
                    Tag ret = new Tag();
                    ret.Name = t.CanonicalName;
                    ret.TargetCommitHash = AbbreviateHash(t.PeeledTarget);
                    yield return ret;
                }
            }
        }
    }
}