export function salary(type) {
  switch (type) {
    case 1:
      return "کمتراز 1 میلیون تومان";

    case 2:
      return "1 تا 2.5 میلیون تومان";

    case 3:
      return "2.5 تا 3.5 میلیون تومان";

    case 4:
      return "3.5 تا 5 میلیون تومان";

    case 5:
      return "5 تا 8 میلیون تومان";

    case 6:
      return "بیشتراز 8 میلیون تومان";

    case 7:
      return "بصورت توافقی";

    case 8:
      return "قانون کار";

    default:
       return "مهم نیست";
  }
}
