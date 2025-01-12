export interface AvailabilityByDay {
  day: number;
  state: number;
}

const stateMapping: { [key: number]: string } = {
  0: 'inPast',
  1: 'today',
  2: 'free',
  3: 'allBooked',
  4: 'closed',
};
export function getState(state: number): string {
  return stateMapping[state] || 'unknown';
}
