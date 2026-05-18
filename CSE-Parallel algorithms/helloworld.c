#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>

// Global variable
int thread_count;

// Thread function prototype
void* Hello(void* rank);

// Main function
int main(int argc, char* argv[]) {
    long thread;
    pthread_t* thread_handles;

    if (argc < 2) {
        printf("Usage: %s <number_of_threads>\n", argv[0]);
        return 1;
    }

    thread_count = strtol(argv[1], NULL, 10);

    thread_handles = malloc(thread_count * sizeof(pthread_t));

    for (thread = 0; thread < thread_count; thread++)
        pthread_create(&thread_handles[thread], NULL, Hello, (void*)thread);

    printf("Hello world from the main thread!\n");

    for (thread = 0; thread < thread_count; thread++)
        pthread_join(thread_handles[thread], NULL);

    free(thread_handles);

    return 0;
}

// Thread function
void* Hello(void* rank) {
    long my_rank = (long)rank;

    printf("Hello world, from thread %ld of %d!\n", my_rank, thread_count);

    return NULL;
}
