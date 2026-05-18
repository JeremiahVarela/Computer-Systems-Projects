#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <mpi.h>

int main(int argc, char** argv) {
    int commsize;
    int my_rank;

    MPI_Init(&argc, &argv);

    MPI_Comm_size(MPI_COMM_WORLD, &commsize);
    MPI_Comm_rank(MPI_COMM_WORLD, &my_rank);

    int darts_to_throw = 1000000;

    if (argc >= 2) {
        darts_to_throw = atoi(argv[1]);
    }

    srand(time(NULL) + my_rank);

    int local_darts_to_throw = darts_to_throw / commsize;

    if (my_rank == commsize - 1) {
        local_darts_to_throw += darts_to_throw % commsize;
    }

    int darts_within_dartboard = 0;
    int global_darts_within_dartboard = 0;

    double start_time, stop_time, local_runtime, global_max_runtime;

    MPI_Barrier(MPI_COMM_WORLD);
    start_time = MPI_Wtime();

    for (int i = 0; i < local_darts_to_throw; i++) {
        double rand_x = ((double) rand() / RAND_MAX) * 2.0 - 1.0;
        double rand_y = ((double) rand() / RAND_MAX) * 2.0 - 1.0;

        double dist_squared = rand_x * rand_x + rand_y * rand_y;

        if (dist_squared <= 1.0) {
            darts_within_dartboard++;
        }
    }

    MPI_Barrier(MPI_COMM_WORLD);
    stop_time = MPI_Wtime();

    local_runtime = stop_time - start_time;

    MPI_Reduce(
        &local_runtime,
        &global_max_runtime,
        1,
        MPI_DOUBLE,
        MPI_MAX,
        0,
        MPI_COMM_WORLD
    );

    MPI_Reduce(
        &darts_within_dartboard,
        &global_darts_within_dartboard,
        1,
        MPI_INT,
        MPI_SUM,
        0,
        MPI_COMM_WORLD
    );

    if (my_rank == 0) {
        double global_pi_est = 4.0 * (double)global_darts_within_dartboard / darts_to_throw;

        printf("Total darts thrown: %d\n", darts_to_throw);
        printf("Processes used: %d\n", commsize);
        printf("Global darts inside circle: %d\n", global_darts_within_dartboard);
        printf("The global estimated value of pi is %f\n", global_pi_est);
        printf("Runtime based on slowest process: %f seconds\n", global_max_runtime);
    }

    MPI_Finalize();

    return 0;
}
