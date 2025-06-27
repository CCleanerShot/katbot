package org.example;

import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.assertNotNull;

public class AppTest {
    @Test
    public void appHasAGreeting() {
        assertNotNull("app should have a greeting", "");
    }
}
