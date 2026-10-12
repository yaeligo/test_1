using System.Collections.Concurrent;
using GithubSearch.Server.Models;

namespace GithubSearch.Server.Services;

public interface IBookmarkService
{
    void AddBookmark(string userId, RepositoryItem item);
    void RemoveBookmark(string userId, long repoId);
    List<RepositoryItem> GetBookmarks(string userId);
}

public class SessionBookmarkService : IBookmarkService
{
    // שמירת ה-Bookmarks בזיכרון של השרת פר יוזר (Thread-safe)
    private readonly ConcurrentDictionary<string, List<RepositoryItem>> _userSessions = new();

    public void AddBookmark(string userId, RepositoryItem item)
    {
        var bookmarks = _userSessions.GetOrAdd(userId, _ => new List<RepositoryItem>());
        lock (bookmarks)
        {
            if (!bookmarks.Any(b => b.Id == item.Id))
            {
                bookmarks.Add(item);
            }
        }
    }

    public void RemoveBookmark(string userId, long repoId)
    {
        if (_userSessions.TryGetValue(userId, out var bookmarks))
        {
            lock (bookmarks)
            {
                bookmarks.RemoveAll(b => b.Id == repoId);
            }
        }
    }

    public List<RepositoryItem> GetBookmarks(string userId)
    {
        if (_userSessions.TryGetValue(userId, out var bookmarks))
        {
            lock (bookmarks)
            {
                return bookmarks.ToList();
            }
        }
        return new List<RepositoryItem>();
    }
}
