import { describe, expect, it } from 'vitest';
import { businessConfig, createWhatsAppUrl } from './business';

describe('createWhatsAppUrl', () => {
  it('uses the centralized number and URL-encodes the message', () => {
    const url = createWhatsAppUrl('Hola, busco modelo A&B.');
    expect(url).toBe(`https://wa.me/${businessConfig.whatsapp}?text=Hola%2C%20busco%20modelo%20A%26B.`);
  });
});
