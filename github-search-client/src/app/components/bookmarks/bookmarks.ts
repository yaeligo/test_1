
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { BookmarkService } from '../../services/bookmark';

@Component({
  selector: 'app-bookmarks',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './bookmarks.html',
  styleUrl: './bookmarks.scss'
})
export class BookmarksComponent implements OnInit {
  constructor(public bookmarkService: BookmarkService) {}

  
ngOnInit(): void {
  this.bookmarkService.refreshBookmarks().subscribe();
}

  removeBookmark(id: number): void {
    this.bookmarkService.removeBookmark(id);
  }
}
