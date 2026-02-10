import * as z from 'zod/mini';

export const PatientId = z.uuid();
export type PatientId = z.infer<typeof PatientId>;

export const PatientAddressId = z.uuid();
export type PatientAddressId = z.infer<typeof PatientAddressId>;
