
module P2.Platform.ContactManagement.Dto {
    
    export interface Address {
        
        /*  */
        id: number;
        /* Street name */
        addressLine1: string;
        /*  */
        addressLine2: string;
        /*  */
        city: string;
        /*  */
        stateProvince: string;
        /*  */
        postalCode: string;
        /*  */
        county: string;
        /*  */
        addressType: string;
        /*  */
        isPrimary: boolean;
    }
}