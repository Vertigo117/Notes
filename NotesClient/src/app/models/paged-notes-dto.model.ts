import { NoteDto } from "./note-dto.model";

export interface PagedNotesDto {
    total: number;
    notes: NoteDto[];
}