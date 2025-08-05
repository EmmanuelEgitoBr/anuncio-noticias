import { Routes } from '@angular/router';
import { NewsComponent } from '../components/news/news.component';
import { NewsCreateComponent } from '../components/news-create/news-create.component';

export const routes: Routes = [
    { path: 'news', component: NewsComponent },
    { path: 'news/create', component: NewsCreateComponent },
    { path: '', redirectTo: 'news', pathMatch: 'full' }
];
