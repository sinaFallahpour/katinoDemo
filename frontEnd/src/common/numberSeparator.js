export function numberSeparator(x) {
  const toRial = x * 1;
  return toRial?.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
