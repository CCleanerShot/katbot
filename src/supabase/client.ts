import { createClient, SupabaseClient } from '@supabase/supabase-js'
import { BotEnvironment } from '../environments';
import { Database } from './types';

class CustomSupabaseClient {
    client: SupabaseClient<Database>
    constructor() {
        this.client = createClient<Database>(BotEnvironment.SUPABASE_URL, BotEnvironment.DISCORD_PUBLIC_KEY);
    }
}

export const supabaseClient = new CustomSupabaseClient();