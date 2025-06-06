<template>
  <v-container v-if="isAuthenticated" class="py-10">
    <v-row justify="center">
      <v-col cols="12" sm="8" md="6">
        <v-card>
          <v-card-title>Create New Post</v-card-title>
          <v-card-text>
            <v-form @submit.prevent="submitPost" ref="form">
              <v-text-field
                v-model="title"
                label="Title"
                required
                outlined
              ></v-text-field>

              <v-textarea
                v-model="content"
                label="Content"
                rows="6"
                outlined
                required
              ></v-textarea>

              <v-btn
                type="submit"
                color="primary"
                :loading="loading"
                :disabled="loading"
                class="mt-4"
                block
              >
                Submit Post
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

            <v-alert
              v-if="success"
              type="success"
              class="mt-4"
              border="start"
              variant="outlined"
            >
              Post created successfully!
            </v-alert>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>

  <v-container v-else class="py-10 text-center">
    <v-alert
      type="warning"
      border="start"
      variant="outlined"
    >
      You must be logged in to create a post. <router-link to="/login">Login here</router-link>
    </v-alert>
  </v-container>
</template>

<script>
export default {
  data() {
    return {
      title: '',
      content: '',
      loading: false,
      error: null,
      success: false,
      isAuthenticated: false,
    }
  },
  mounted() {
    const token = localStorage.getItem('jwtToken')
    this.isAuthenticated = !!token
  },
  methods: {
    async submitPost() {
      this.error = null
      this.success = false
      this.loading = true

      const token = localStorage.getItem('jwtToken')
      if (!token) {
        this.error = 'You must be logged in to submit a post.'
        this.loading = false
        return
      }

      try {
        const response = await fetch("https://localhost:7189/api/forum/CreatePost", {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify({
            title: this.title,
            content: this.content,
          }),
        })

        if (!response.ok) {
          const errorText = await response.text()
          throw new Error(errorText || 'Failed to create post')
        }

        this.success = true
        this.title = ''
        this.content = ''
      } catch (err) {
        this.error = err.message
      } finally {
        this.loading = false
      }
    },
  },
}
</script>
