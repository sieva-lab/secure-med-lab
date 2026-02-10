import * as z from 'zod/mini';
import { PatientId } from './strongly-typed-ids.model';

export const PatientOverviewResponse = z.strictObject({
	id: PatientId,
	firstName: z.string(),
	lastName: z.string(),
});
export type PatientOverviewResponse = z.infer<typeof PatientOverviewResponse>;
