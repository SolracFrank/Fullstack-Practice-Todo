import { DateTime } from "luxon";

export const DateConverter = (fecha: string): string  => {
  try {
    const date = DateTime.fromISO(fecha!);

    if (!date.isValid) {
      return DateTime.now().toUTC().toISO()!;
    }

    const dateConverted = date.toUTC().toISO();
    return dateConverted || DateTime.now().toUTC().toISO()!;
  } catch (err) {
    return DateTime.now().toUTC().toISO()!;
  }
};
