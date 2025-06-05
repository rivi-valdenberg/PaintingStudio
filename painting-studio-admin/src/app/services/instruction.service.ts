import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Instruction, InstructionCreate, InstructionUpdate } from '../models/instruction.model';

@Injectable({
  providedIn: 'root'
})
export class InstructionService {
  private apiUrl = `${environment.apiUrl}/instructions`;

  constructor(private http: HttpClient) { }

  getInstructions(): Observable<Instruction[]> {
    return this.http.get<Instruction[]>(this.apiUrl);
  }

  getInstruction(id: number): Observable<Instruction> {
    return this.http.get<Instruction>(`${this.apiUrl}/${id}`);
  }

  getInstructionsByItem(itemId: number): Observable<Instruction[]> {
    return this.http.get<Instruction[]>(`${this.apiUrl}/item/${itemId}`);
  }

  createInstruction(instruction: InstructionCreate): Observable<Instruction> {
    const formData = new FormData();
    formData.append('itemId', instruction.itemId.toString());
    formData.append('stepNumber', instruction.stepNumber);
    formData.append('description', instruction.description);
    
    if (instruction.image) {
      formData.append('image', instruction.image);
    }

    return this.http.post<Instruction>(this.apiUrl, formData);
  }

  updateInstruction(id: number, instruction: InstructionUpdate): Observable<void> {
    const formData = new FormData();
    formData.append('stepNumber', instruction.stepNumber);
    formData.append('description', instruction.description);
    
    if (instruction.image) {
      formData.append('image', instruction.image);
    }

    return this.http.put<void>(`${this.apiUrl}/${id}`, formData);
  }

  deleteInstruction(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}