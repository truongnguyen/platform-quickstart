
module P2.Platform.ContactManagement.Dto {
    
    export interface Contact {
        
        /* Identifier */
        id: number;
        /* First name */
        firstName: string;
        /* Last name */
        lastName: string;
        /*  */
        phoneNumber: string;
        /*  */
        emailAddress: string;
        /*  */
        organization: string;
        /*  */
        tags: string[];
        /*  */
        createdDate: Date;
        /*  */
        newName: string;
        /*  */
        addresses: Address[];
    }
}