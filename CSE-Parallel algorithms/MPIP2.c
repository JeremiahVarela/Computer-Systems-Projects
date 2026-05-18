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
        for (int q = 1; q < comm_sz; q++) {
            sprintf(greeting, "Process 0 says hello to you!\n");
            MPI_Send(greeting, strlen(greeting) + 1, MPI_CHAR, q, 0, MPI_COMM_WORLD);
            printf("Process 0 has sent a message to process %d.\n", q);
        }
    } else {
        MPI_Recv(greeting, MAX_STRING, MPI_CHAR, 0, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
        printf("Process %d received: %s", my_rank, greeting);
    }

    MPI_Finalize();
    return 0;
}
