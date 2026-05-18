#include <stdio.h>
#include <string.h>
#include <mpi.h>

const int MAX_STRING = 100;

int main(void) {
    char greeting[MAX_STRING];
    int comm_sz;
    int my_rank;

    MPI_Init(NULL, NULL);
    MPI_Comm_size(MPI_COMM_WORLD, &comm_sz);
    MPI_Comm_rank(MPI_COMM_WORLD, &my_rank);

    if (my_rank == 0) {
        sprintf(greeting, "Process 0 says hello to you!\n");
    }

    MPI_Bcast(greeting, MAX_STRING, MPI_CHAR, 0, MPI_COMM_WORLD);

    if (my_rank != 0) {
        printf("Process %d received: %s", my_rank, greeting);
    } else {
        printf("Process 0 has successfully broadcasted a message.\n");
    }

    MPI_Finalize();
    return 0;
}
