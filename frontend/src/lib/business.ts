export const businessConfig = Object.freeze({
  name: 'Control Eléctrico Obrera',
  phone: '55 5588 3472',
  whatsapp: '525555883472',
  location: 'Obrera, CDMX',
  business: 'Material eléctrico industrial',
  requests24h: 'Recibimos solicitudes las 24 horas.',
});

export function createWhatsAppUrl(message = 'Hola, quiero información sobre material eléctrico.'): string {
  return `https://wa.me/${businessConfig.whatsapp}?text=${encodeURIComponent(message)}`;
}
