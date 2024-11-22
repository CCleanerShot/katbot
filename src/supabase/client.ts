import { createClient, SupabaseClient } from '@supabase/supabase-js'
import { BotEnvironment } from '../environment';
import { Database } from './types';

class CustomSupabaseClient {
    client: SupabaseClient<Database>
    constructor() {
        this.client = createClient<Database>(BotEnvironment.SUPABASE_URL, BotEnvironment.SUPABASE_PUBLIC_KEY);
    }
}

export const supabaseClient = new CustomSupabaseClient();