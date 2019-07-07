function [x] = bsub(U,y)
%takes in an upper-triangular matrix U and column vector y
%returns column vector x that is the solution to Ux=y

[N,N] = size(U);%get size of U
x = zeros(N,1); % initialize x
x(N) = y(N)/U(N,N);

for i = N-1:-1:1
   x(i) = (y(i)-(U(i,i+1:N)*x(i+1:N)))/U(i,i); 
end


end

