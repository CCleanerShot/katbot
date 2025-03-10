export const utilityServer = {
    cleanMongoResponse: (data: any[]) => {
        for (const item of data) 
        {
            if(item._id)
                delete item._id;

            // TODO: add more if needed
        }
    }
} as const;