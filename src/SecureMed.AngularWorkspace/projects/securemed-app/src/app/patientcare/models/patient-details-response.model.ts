import * as z from 'zod/mini';
import { Address } from './address.model';
import { PatientAddressId, PatientId } from './strongly-typed-ids.model';


const HomeAddress = z.strictObject({
	...Address.def.shape,
	id: PatientAddressId,
});

const ShippingAddress = z.strictObject({
	...Address.def.shape,
	id: PatientAddressId,
	note: z.string(),
});
const BillingAddress = z.strictObject({
	...Address.def.shape,
	id: PatientAddressId,
});

export const PatientDetailsResponse = z.strictObject({
	id: PatientId,
	firstName: z.string(),
	lastName: z.string(),
	address: HomeAddress,
    billingAddresses: z.array(BillingAddress),
	shippingAddresses: z.array(ShippingAddress),

});
export type PatientDetailsResponse = z.infer<typeof PatientDetailsResponse>;
