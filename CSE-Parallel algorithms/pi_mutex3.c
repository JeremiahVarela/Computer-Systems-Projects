// pi-estimating program,written in c, with mutex lox for enforcing mutual exclusion 
// With local sum calculation for better performance
// With bugfix for allowing arbitrary thread counts
#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>



// Global variables, denotes how many terms there are, and the sum for pi
double sum = 0.0; // final sum of int
int n;// number of terms to add
int thread_count; // Number of threads the program will have
pthread_mutex_t mutex;  //mutex lock

//function to be called by threads
void* Thread_sum(void* rank);

// Main function
int main(int argc, char* argv[]) {
  // Thread handle
  pthread_t* thread_handles;
  
  // Initialize mutex
  pthread_mutex_init(&mutex, NULL);
  
  // Get a number of terms ti add from the command line 
  thread_count = strtol(argv[1], NULL, 10);
  n = strtol(argv[1], NULL, 10);
  
  // Allocate memory for the thread handles
  thread_handles = malloc(thread_count * sizeof(pthread_t));
  
  long thread;
  for(thread = 0; thread < thread_count; thread++) {
    pthread_create(&thread_handles[thread], NULL, Thread_sum, (void*)thread);
  }
  
  for (thread = 0; thread < thread_count; thread++) {
    pthread_join(thread_handles[thread], NULL);
  }
  
  // Destroy the mutex
  pthread_mutex_destroy(&mutex);
  
  //deallocate mem
  free (thread_handles);
  double pi = 4.0 * sum;
  printf("The estimated value of pi is %f.\n", pi);
return 0;
}

void* Thread_sum(void* rank) {
  long my_rank = (long) rank;
  double factor;
  long long i;
  long long my_n = n/ thread_count;
  long long my_first_i = my_n *my_rank;
  long long my_last_i = my_first_i + my_n;
  
  // If this is the last thread, have my_last_i be n instead
  if (my_rank +1 ==thread_count) {
    my_last_i = n;
  }
  
  printf("Thread %ld is adding %lld terms (%lld to %lld).\n",
    my_rank, my_last_i - my_first_i, my_first_i, my_last_i);
  // Make sure the sign for first term is correct
  if (my_first_i % 2 == 0) factor = 1.0;
  else factor = -1.0;

  // Add my_n consecutive terms, from my_first_i to my_last_i
  // Add terms to a local sum
  double my_sum = 0;
  for (i = my_first_i; i < my_last_i; i++, factor = -factor) {
  my_sum += factor / (2 * i + 1);
  }
  // Acquire lock before entering critical section
  pthread_mutex_lock(&mutex);
  
  //critical section
   sum += my_sum;
   
  // Release the lock after exiting the critical section
  pthread_mutex_unlock(&mutex);
  return NULL;
}

