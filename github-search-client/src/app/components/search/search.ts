import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { GithubService } from '../../services/github';
import { BookmarkService } from '../../services/bookmark';
import { RepositoryItem } from '../../models/repository.model';
import { switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './search.html',
  styleUrl: './search.scss'
})
export class SearchComponent {
  searchQuery = '';
  repositories: RepositoryItem[] = [];
  isLoading = false;
  hasSearched = false;

  constructor(
    private githubService: GithubService,
    public bookmarkService: BookmarkService
  ) {}

  
onSearch(): void {
  if (!this.searchQuery.trim()) return;

  this.isLoading = true;
  this.hasSearched = true;

  this.bookmarkService.refreshBookmarks().pipe(
    switchMap(() =>
      this.githubService.searchRepositories(this.searchQuery)
    )
  ).subscribe({
    next: (res: any) => {
      this.repositories = res.items || [];
      this.isLoading = false;
    },
    error: error => {
      console.error('Search failed', error);
      this.repositories = [];
      this.isLoading = false;
    }
  });
}

  
toggleBookmark(repo: RepositoryItem): void {
  const isSaved = this.bookmarkService.isBookmarked(repo.id);

  if (isSaved) {
    this.bookmarkService.removeBookmark(repo.id);
  } else {
    this.bookmarkService.addBookmark(repo);
  }
}

}