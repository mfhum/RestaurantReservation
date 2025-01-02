export const formatDate = (date: Date) => {
  console.log("standard date:", date);
  date = toLocalDateTime(date);
  console.log("formatted date:", date);

  const year = String(date.getFullYear());
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
};

export const formatTime = (date: Date) => {
  date = toLocalDateTime(date);
  const hours = String(date.getHours()).padStart(2, '0');
  const minutes = String(date.getMinutes()).padStart(2, '0');
  return `${hours}:${minutes}`;
};

export const toLocalDateTime = (date: Date) => {
  console.log("FUNCTION standard date:", date);
  const localDate = new Date(date.toLocaleString('en-US', { timeZone: 'Europe/Zurich' }));
  console.log("FUNCTION formatted date:", localDate);
  return localDate;
};