import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Item, ItemCreate, ItemUpdate } from '../models/item.model';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  private apiUrl = `${environment.apiUrl}/items`;

  constructor(private http: HttpClient) { }

  getItems(): Observable<Item[]> {
    return this.http.get<Item[]>(this.apiUrl);
  }

  getItem(id: number): Observable<Item> {
    return this.http.get<Item>(`${this.apiUrl}/${id}`);
  }

  createItem(item: ItemCreate): Observable<Item> {
    const formData = new FormData();
    formData.append('name', item.name);
    formData.append('description', item.description);
    
    if (item.image) {
      formData.append('image', item.image);
    }

    return this.http.post<Item>(this.apiUrl, formData);
  }

  updateItem(id: number, item: ItemUpdate): Observable<void> {
    const formData = new FormData();
    formData.append('name', item.name);
    formData.append('description', item.description);
    
    if (item.image) {
      formData.append('image', item.image);
    }

    return this.http.put<void>(`${this.apiUrl}/${id}`, formData);
  }

  deleteItem(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}