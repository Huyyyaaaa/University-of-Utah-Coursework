function [y] = fsub(L,d)
%takes in a unit-lower matrix L and column vector d
% and returns a column vector y to Ly=d

[M,N]=size(L);%get size of L
y=d; %initialize y
for i=2:N
   y(i)=d(i)-(L(i,1:i-1)*y(1:i-1)); 
end

end

