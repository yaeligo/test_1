
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { RepositoryItem } from '../models/repository.model';

@Injectable({
  providedIn: 'root'
})
export class BookmarkService {
  private readonly apiUrl = 'http://localhost:5000/api/bookmarks';

  private bookmarksSubject =
    new BehaviorSubject<RepositoryItem[]>([]);

  bookmarks$ = this.bookmarksSubject.asObservable();

  constructor(private http: HttpClient) {}

  refreshBookmarks(): Observable<RepositoryItem[]> {
    return this.http.get<RepositoryItem[]>(this.apiUrl).pipe(
      catchError(error => {
        console.error('Failed to load bookmarks', error);
        return of([]);
      }),
      tap(items => this.bookmarksSubject.next(items))
    );
  }

  getBookmarks(): RepositoryItem[] {
    return this.bookmarksSubject.value;
  }

  isBookmarked(id: number): boolean {
    return this.bookmarksSubject.value.some(
      item => Number(item.id) === Number(id)
    );
  }

  addBookmark(item: RepositoryItem): void {
    if (this.isBookmarked(item.id)) {
      return;
    }

    this.http.post(this.apiUrl, item).subscribe({
      next: () => {
        this.refreshBookmarks().subscribe();
      },
      error: error => console.error('Failed to add bookmark', error)
    });
  }

  removeBookmark(id: number): void {
    this.http.delete(`${this.apiUrl}/${id}`).subscribe({
      next: () => {
        this.refreshBookmarks().subscribe();
      },
      error: error => console.error('Failed to remove bookmark', error)
    });
  }
}
