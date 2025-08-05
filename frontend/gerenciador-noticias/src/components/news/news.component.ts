import { Component, OnInit, TemplateRef } from '@angular/core';
import { News, NewsService } from './news.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-news',
  imports: [CommonModule, FormsModule, NgbModule],
  templateUrl: './news.component.html',
})
export class NewsComponent implements OnInit {
  newsList: News[] = [];
  selectedNews: News = this.initNews();

  constructor(
    private newsService: NewsService,
    private modalService: NgbModal
  ) {}

  ngOnInit() {
    this.loadNews();
  }

  loadNews() {
    this.newsService.getAll().subscribe(data => this.newsList = data);
  }

  openEditModal(content: TemplateRef<any>, news: News) {
    this.selectedNews = { ...news };
    this.modalService.open(content);
  }

  save() {
    if (this.selectedNews.id) {
      this.newsService.update(this.selectedNews.id, this.selectedNews).subscribe(() => this.loadNews());
    }
  }

  delete(id: string) {
    this.newsService.delete(id).subscribe(() => this.loadNews());
  }

  private initNews(): News {
    return {
      hat: '',
      title: '',
      text: '',
      author: '',
      image: '',
      link: '',
      status: 0
    };
  }
}
