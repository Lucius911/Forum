<template>
  <v-container class="py-10">
    <v-row justify="center">
      <v-col cols="12" sm="8" md="6">
        <v-card>
          <v-card-title class="text-h5">Register</v-card-title>
          <v-card-text>
            <v-form @submit.prevent="registerUser" ref="form" validate-on="submit">
              <v-text-field v-model="firstName" label="First Name" prepend-icon="mdi-account" required />

              <v-text-field v-model="lastName" label="Last Name" prepend-icon="mdi-account" required />

              <v-text-field v-model="displayName" label="Display Name" prepend-icon="mdi-account-circle" required />

              <v-text-field v-model="email" label="Email" type="email" prepend-icon="mdi-email" required />

              <v-text-field v-model="password" label="Password" type="password" prepend-icon="mdi-lock" required />

              <v-text-field v-model="confirmPassword" label="Confirm Password" type="password"
                prepend-icon="mdi-lock-check" :error="passwordMismatch"
                :error-messages="passwordMismatch ? ['Passwords do not match'] : []" required />

              <v-checkbox v-model="isModerator" label="Register as Moderator" class="mt-2" />

              <v-btn type="submit" color="primary" :loading="loading" :disabled="loading" class="mt-4" block>
                Register
              </v-btn>
            </v-form>

            <v-alert v-if="error" type="error" class="mt-4" border="start" variant="outlined">
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
  name: 'RegisterPage',
  data() {
    return {
      firstName: '',
      lastName: '',
      displayName: '',
      email: '',
      password: '',
      confirmPassword: '',
      loading: false,
      error: null,
      isModerator: false,
    };
  },
  computed: {
    passwordMismatch() {
      return this.password && this.confirmPassword && this.password !== this.confirmPassword;
    },
  },
  methods: {
    async registerUser() {
      if (this.passwordMismatch) {
        this.error = 'Passwords do not match';
        return;
      }

      this.loading = true;
      this.error = null;

      try {
        const response = await fetch('https://localhost:7189/api/account/register', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            firstName: this.firstName,
            lastName: this.lastName,
            displayName: this.displayName,
            email: this.email,
            password: this.password,
            confirmPassword: this.confirmPassword,
            isModerator: this.isModerator,
          }),
        });

        if (!response.ok) {
          const err = await response.json();
          throw new Error(err.message || 'Registration failed');
        }
        const data = await response.json();
        console.log('Registration successful:', data);
        // Handle success (e.g., store JWT, redirect, show toast)
        this.$router.push('/login');
      } catch (err) {
        this.error = err.message;
      } finally {
        this.loading = false;
      }
    },
  },
};
</script>
