import * as z from 'zod/mini';

export const CreatePatientCommand = z.strictObject({
	firstName: z.string(),
	lastName: z.string(),
    // insz: z.string(),
	address: z.nullable(
		z.strictObject({
			street: z.string(),
			city: z.string(),
			zipCode: z.string(),
		}),
    ),
    billingAddress: z.nullable(
		z.strictObject({
			street: z.string(),
			city: z.string(),
			zipCode: z.string(),
		}),
	),
	shippingAddress: z.nullable(
		z.strictObject({
			street: z.string(),
			city: z.string(),
			zipCode: z.string(),
			note: z.nullable(z.string()),
		}),
	),
});
export type CreatePatientCommand = z.infer<typeof CreatePatientCommand>;
