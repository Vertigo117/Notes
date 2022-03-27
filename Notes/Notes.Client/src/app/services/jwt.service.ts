import { Injectable } from '@angular/core';
import { Base64 } from 'js-base64';
import { Payload } from '../models/payload.model';
import { ACCESS_TOKEN_KEY } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  constructor() { }

  public getPayload(): Payload | null {
    let jwt: string | null = localStorage.getItem(ACCESS_TOKEN_KEY);

    if (!jwt) {
      return null;
    }

    let parts: string[] = jwt.split('.');
    const payloadIndex = 1;
    return JSON.parse(Base64.decode(parts[payloadIndex])) as Payload;
  }
}
