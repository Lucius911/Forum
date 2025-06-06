<template>
  <v-container class="py-10">
    <v-row justify="center">
      <v-col cols="12" sm="6" md="4">
        <v-card>
          <v-card-title class="text-h5">Login</v-card-title>
          <v-card-text>
            <v-form @submit.prevent="loginUser" ref="form">
              <v-text-field
                v-model="email"
                label="Email"
                prepend-icon="mdi-email"
                type="email"
                required
              />

              <v-text-field
                v-model="password"
                label="Password"
                prepend-icon="mdi-lock"
                type="password"
                required
              />

              <v-btn
                type="submit"
                color="primary"
                :loading="loading"
                :disabled="loading"
                class="mt-4"
                block
              >
                Login
              </v-btn>
            </v-form>

            <v-alert
              v-if="error"
              type="error"
              class="mt-4"
              border="start"
              variant="outlined"
            >
              {{ error }}
            </v-alert>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  name: 'LoginPage',
  data() {
    return {
      email: '',
      password: '',
      loading: false,
      error: null,
    };
  },
  methods: {
    async loginUser() {
      this.loading = true;
      this.error = null;

      try {
        const response = await fetch("https://localhost:7189/api/account/login", {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            email: this.email,
            password: this.password,
          }),
        });

        if (!response.ok) {
          const err = await response.json();
          throw new Error(err.message || 'Login failed');
        }

        const data = await response.text();
        // Store JWT token in localStorage/sessionStorage or Vuex store
        localStorage.setItem('jwtToken', data);
        // Redirect or do other login success logic
        this.$router.push('/');

      } catch (err) {
        this.error = err.message;
      } finally {
        this.loading = false;
      }
    },
  },
};
</script>
